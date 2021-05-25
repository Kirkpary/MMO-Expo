
# MMO Expo
[Link to prototype](https://kirkpary.github.io/MMO-Expo/prototype/)

### To Build Project
1. Download Unity version 2020.3.5f1 and install the WebGL option.
2. Clone the source code on Github to your computer.
3. Make a [Photon Unity 3D Networking](https://www.photonengine.com/en-US/Photon) account and get API keys for Photon Chat and Photon Pun.
4. Open the project source code in Unity.
5. Search up the 'PhotonServerSettings' file within the 'Assets' folder. Edit 'AppIdRealtime' and 'AppIdChat' within the file to include your API keys.
6. Hit 'Play' to build the project locally.
7. Under 'Build Settings', choose WebGL as your platform. Then, 'Build and Run' to create a WebGL build of the project. This WebGL build can be hosted online.

### Overview of Source Code
Most of the folders within the MMO-Expo main directory are folders that are generated by Unity. The folder that you would make changes to is the 'Assets' folder. This 'Assets' folder holds all of the assets and scripts that we created. Key sub-folders within this folder includes:\
**Logos**: Contains the image of the splash logo when the project is first loaded.\
**Prefabs**: Contains the prefabs that we use within scenes.\
**Resources** -> **Avatars**: Contains the models of our non-trivial characters.\
**Scenes**: Contains the scenes of every page in the project.\
**Scripts**: Contains all the scripts in this project.\
**SkySerie Freebie**: Contains all the skyboxes for the scenes.\
**StreamingAssets**: Contains the JSON files that some of our scripts read from.

### Links to More Resources for the MMO Expo:
[Initial Design Draft](https://docs.google.com/document/d/1tzyd4i_lpUAIDl0RLQdcVqzDxQ98jQdrCNcf3G05Zwk/edit?usp=sharing)\
[Mid-project Retrospective](https://docs.google.com/document/d/1zvwvDPPP4_o5zIjxVNu79GfNFOGAf2RShcIgcptLVdc/edit?usp=sharing)\
[Final Design Document](https://docs.google.com/document/d/1hFkAjG0oVUYCVBS6ftjstKus2_QDqOdR61FU2edIRVA/edit?usp=sharing)
