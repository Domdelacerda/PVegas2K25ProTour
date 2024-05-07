Team Name: Compu-Force
Project Name: PVegas Tour 2K25
Team Members: Daniel Borgh, Dominic De La Cerda, Cameron Pokorny, Jestin Sertle, Andrew Suetholz
Build Instructions: Before starting the building process, you will need to have installed Visual Studio 2022 in order to be able to open the project. In the Visual Studio installer, make sure to install the .Net desktop development workload so that it can execute c# applications on your desktop.

Once that is downloaded, you can access our project in our repository inside of the PVegas2K25ProTour folder. Inside of that folder, you will see the Visual Studio solution PVegas2K25ProTour.sln, which you can then open in Visual Studio. Once the project is open, right click on the PVegas2K25ProTour project in the solution explorer and click on properties.

Inside of the project properties, click on the build page. In the platform target dropdown, select x86, and under the Optimize code section, check the box for Release. After that, in the Output tab of the build page, specify a Base output path where you want the game to be stored on your computer. In the toolbar at the top of the screen, there are 3 dropdown menus specifying build type, target platform, and project. For the first box, select release, and for the second box, select x86. 

The last step is to build the solution by clicking on the build tab in the toolbar and clicking build solution. Once the solution is finished building, the folder you specified in the output path field will now contain the executable file for our game, PVegas2K25ProTour.exe, which you can run by double clicking on it in the file explorer.

Level 5 contains a slight bug, where the 2nd downslope does not act as intended. This happens due to the logic of the obstacle. This would be easy fix, but due to time constraints it was unable to be fixed.
Obstacles have a bug with collision, where if ball is hit in a perfect way can cause ball to appear to be stuck to whatever it has collided with whether it is an obstacle or border. Due to the construction of our collision, it simple would have been way to large of an undertaking to try and accomplish this debugging. Implementing Full screen doesn't have a bug, but does contains a performance issue, where it may take a while to load Full screen. Coin updating has an issue, where it doen't always update. This issue wasn't fixed we struggled to locate source of the issue.

One of the most prominent design dificencies was our game control class. It became way to bloated, and defintitly was in need of a redesign. It was in desperate need of being broken up into its many functions that it had absorbed. 

(ADD test code coverage regeneration)
