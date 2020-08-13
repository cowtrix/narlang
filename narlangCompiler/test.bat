mkdir "C:\Users\gbfinnes\Documents\repos\narlang\narlangCompiler\test\bin\x64\Debug\netcoreapp3.1\templates"
if x64 == x64 (
    xcopy "C:\Users\gbfinnes\Documents\repos\narlang\narlangCompiler\test\templates" "C:\Users\gbfinnes\Documents\repos\narlang\narlangCompiler\test\bin\x64\Debug\netcoreapp3.1\templates"  /E /H /Y
) else if $(PlatformName) == x64 (
    cp -r "$(ProjectDir)templates C:\Users\gbfinnes\Documents\repos\narlang\narlangCompiler\test\bin\x64\Debug\netcoreapp3.1\templates"
)