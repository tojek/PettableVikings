#!/usr/bin/env python3
"""Build PettableVikings and zip it as a Thunderstore/Hexium/Gale package.

Usage:
  ./package.py --profile "<Gale profile folder>" [--valheim "<Valheim install>"] [--no-build]
Output: dist/PettableVikings-<version>.zip
"""
import argparse, json, re, struct, subprocess, sys, zipfile
from pathlib import Path

ROOT = Path(__file__).resolve().parent
SRC = ROOT / "PettableVikings"
REL = ROOT / "PettableVikings_Release"

def fail(msg):
    sys.exit(f"ERROR: {msg}")

def validate():
    m = json.loads((REL / "manifest.json").read_text(encoding="utf-8"))
    for k in ("name", "version_number", "description", "website_url", "dependencies"):
        if k not in m:
            fail(f"manifest.json is missing '{k}'")
    if not re.fullmatch(r"[A-Za-z0-9_]+", m["name"]):
        fail("manifest name may only contain letters, digits and underscores")
    if not re.fullmatch(r"\d+\.\d+\.\d+", m["version_number"]):
        fail("version_number must be Major.Minor.Patch")
    if len(m["description"]) > 250:
        fail("description is longer than 250 characters")
    if not m["website_url"]:
        fail("website_url is empty")
    cs = (SRC / "PettableVikings.cs").read_text(encoding="utf-8")
    prj = (SRC / "PettableVikings.csproj").read_text(encoding="utf-8-sig")
    v_cs = re.search(r'ModVersion = "([^"]+)"', cs).group(1)
    v_prj = re.search(r"<Version>([^<]+)</Version>", prj).group(1)
    if not (v_cs == v_prj == m["version_number"]):
        fail(f"version mismatch: .cs={v_cs} .csproj={v_prj} manifest={m['version_number']}")
    png = (REL / "icon.png").read_bytes()
    if png[:8] != b"\x89PNG\r\n\x1a\n" or struct.unpack(">II", png[16:24]) != (256, 256):
        fail("icon.png must be a 256x256 PNG")
    for f in ("README.md", "CHANGELOG.md"):
        if not (REL / f).exists():
            fail(f"{f} is missing")
    return m

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--profile", help="Gale profile folder (contains BepInEx/core)")
    ap.add_argument("--valheim", help="Valheim install folder")
    ap.add_argument("--no-build", action="store_true", help="reuse the existing Release DLL")
    a = ap.parse_args()

    m = validate()
    if not a.no_build:
        if not a.profile:
            fail("--profile is required unless --no-build is given")
        cmd = ["dotnet", "build", str(SRC / "PettableVikings.csproj"), "-c", "Release",
               f"-p:BepInExProfilePath={a.profile}"]
        if a.valheim:
            cmd.append(f"-p:ValheimPath={a.valheim}")
        if subprocess.call(cmd) != 0:
            fail("build failed")

    dlls = list(SRC.glob("bin/Release/**/PettableVikings.dll"))
    if not dlls:
        fail("PettableVikings.dll not found under bin/Release")
    dll = max(dlls, key=lambda p: p.stat().st_mtime)

    out = ROOT / "dist"
    out.mkdir(exist_ok=True)
    zpath = out / f"{m['name']}-{m['version_number']}.zip"
    with zipfile.ZipFile(zpath, "w", zipfile.ZIP_DEFLATED) as z:
        for f in ("manifest.json", "README.md", "CHANGELOG.md", "icon.png"):
            z.write(REL / f, f)
        z.write(dll, "PettableVikings.dll")
    print(f"Wrote {zpath}")
    print("Contents:", ", ".join(zipfile.ZipFile(zpath).namelist()))

if __name__ == "__main__":
    main()
