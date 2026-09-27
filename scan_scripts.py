import os
import re

PROJECT_ROOT = "Assets"
SCRIPT_ROOT = os.path.join(PROJECT_ROOT, "Scripts")

TEXT_EXTENSIONS = {
    ".cs",
    ".prefab",
    ".unity",
    ".asset",
    ".controller",
    ".mat",
    ".anim",
}

def read_text(path):
    try:
        with open(path, "r", encoding="utf-8", errors="ignore") as f:
            return f.read()
    except Exception:
        return ""

def get_guid(script_path):
    meta_path = script_path + ".meta"

    if not os.path.exists(meta_path):
        return None

    text = read_text(meta_path)

    match = re.search(r"^guid:\s*([a-fA-F0-9]+)", text, re.MULTILINE)

    if match:
        return match.group(1)

    return None

def get_class_names(text):
    pattern = r"\bclass\s+([A-Za-z_][A-Za-z0-9_]*)"
    return re.findall(pattern, text)

# ---------------------------------------------------------
# Collect project files
# ---------------------------------------------------------

project_files = []

for root, dirs, files in os.walk(PROJECT_ROOT):
    for filename in files:
        extension = os.path.splitext(filename)[1].lower()

        if extension in TEXT_EXTENSIONS:
            project_files.append(os.path.join(root, filename))

# ---------------------------------------------------------
# Collect scripts
# ---------------------------------------------------------

scripts = []

for root, dirs, files in os.walk(SCRIPT_ROOT):
    for filename in files:
        if filename.endswith(".cs"):
            scripts.append(os.path.join(root, filename))

print()
print("========================================")
print(" Unity Script Usage Scanner")
print("========================================")
print()

used = []
possibly_unused = []
no_meta = []

for script in sorted(scripts):

    script_text = read_text(script)
    class_names = get_class_names(script_text)

    guid = get_guid(script)

    if not guid:
        no_meta.append(script)
        continue

    unity_references = []
    code_references = []

    # -----------------------------------------------------
    # Search Unity GUID references
    # -----------------------------------------------------

    for file in project_files:

        # Don't count the script's own .meta file
        if file == script + ".meta":
            continue

        text = read_text(file)

        if guid in text:
            unity_references.append(file)

    # -----------------------------------------------------
    # Search C# class references
    # -----------------------------------------------------

    for class_name in class_names:

        pattern = re.compile(
            r"\b" + re.escape(class_name) + r"\b"
        )

        for file in project_files:

            if not file.endswith(".cs"):
                continue

            if file == script:
                continue

            text = read_text(file)

            if pattern.search(text):
                code_references.append(file)

    # Remove duplicates
    unity_references = sorted(set(unity_references))
    code_references = sorted(set(code_references))

    if unity_references or code_references:

        used.append(
            (
                script,
                unity_references,
                code_references
            )
        )

    else:

        possibly_unused.append(script)

# ---------------------------------------------------------
# Print results
# ---------------------------------------------------------

print("========== USED / REFERENCED ==========")
print()

for script, unity_refs, code_refs in used:

    print(script)

    if unity_refs:
        print("  Unity references:")
        for ref in unity_refs:
            print("    ->", ref)

    if code_refs:
        print("  C# references:")
        for ref in code_refs:
            print("    ->", ref)

    print()

print()
print("========== POSSIBLY UNUSED ==========")
print()

for script in possibly_unused:
    print(script)

print()
print("========== MISSING META ==========")
print()

for script in no_meta:
    print(script)

print()
print("========================================")
print(f"Total scripts:          {len(scripts)}")
print(f"Referenced scripts:     {len(used)}")
print(f"Possibly unused:        {len(possibly_unused)}")
print(f"Missing .meta:          {len(no_meta)}")
print("========================================")
