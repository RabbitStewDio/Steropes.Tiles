# Note: This is a Windows Assembly (for now)

This test assembly uses Windows DirectX as MonoGame implementation
as the OpenGL version of MonoGame 3.6 will not run as part of a unit 
test as it really, really, really wants to read an application icon
and will refuse to work if it can't.

This is fixed in https://github.com/MonoGame/MonoGame/pull/5764 but
not yet available as an official build. This nasty 'x86' build will
be converted to OpenGL/AnyCPU as soon as MonoGame 3.7 is released.