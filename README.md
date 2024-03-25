# Win2D

## Although I'm not a very proficient C++ developer, many features of Win2D are quite frustrating, so I plan to develop my own. Here is my todo list:

- [x] Add custom font from arbitrary location to fontset
- [x] Add custom font from stream to fontset
- [x] Expose a more comprehensive textformat interface
- [ ] Multithread support for XAML panel

---
The reason why I don't use a WinRT lib project is because the template in Win2D is super useful, and preparing the environment for a WinRT lib sucks.

**The build step is almost the same as the official lib, except for the removal of restrictions in NuGet versions.**

