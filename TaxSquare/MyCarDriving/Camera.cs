
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCM.Service.SeoulPlaza.Content.Autumn
{
    public class AutumnCameraRigFollow : MonoBehaviour
    {
        [SerializeField] private Transform playerT;
        //private Vector3 m_offsetPosition = new Vector3(0f, 1f, 0f);
        private Vector3 m_offsetPosition = new Vector3(-5f, 7f, 5f);
        private Camera mainCamera;
        private Transform targetTransform;

        private void OnEnable()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            moveToCharacter();
        }

        private void Update()
        {
            transform.position = targetTransform.position + m_offsetPosition;
        }

        private void moveToCharacter()
        {
            targetTransform = playerT;
        }
    }
}

=========
Internal build system error. Backend exited with code 2.
STDOUT:
Finished compiling graph: 541 nodes, 1094 flattened edges (1090 ToBuild, 6 ToUse), maximum node priority 30
[  0/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/hr2q_Posix3.lump.cpp
[  1/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/hr2q_Posix4.lump.cpp
[  2/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/4xol_tests2.lump.cpp
[  3/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/4xol_tests0.lump.cpp
[  4/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/psp5_System0.lump.cpp
[  5/534    0s] WriteText Library/Bee/artifacts/Android/ihupg/_dummy_for_header_discovery
[  6/534    0s] WriteText Library/Bee/artifacts/Android/4qqi9/_dummy_for_header_discovery
[  7/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/1t2c_System0.lump.cpp
[  8/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/0c9e_metadata1.lump.cpp
[  9/534    0s] WriteText Library/Bee/artifacts/Android/x6ly2/_dummy_for_header_discovery
[ 10/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/qiql_codegen0.lump.cpp
[ 11/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kebh_vm0.lump.cpp
[ 12/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/td6u_gc0.lump.cpp
[ 13/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/uke6_libil2cpp0.lump.cpp
[ 14/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/9mpy_Win320.lump.cpp
[ 15/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/4xol_tests1.lump.cpp
[ 16/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/jmx4_System3.lump.cpp
[ 17/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/9mpy_Win321.lump.cpp
[ 18/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/y3qr_System0.lump.cpp
[ 19/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/jmx4_System1.lump.cpp
[ 20/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/e7m4_c-api3.lump.cpp
[ 21/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/t81u_System0.lump.cpp
[ 22/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/ed5b_System2.lump.cpp
[ 23/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/66pq_em.Runtime0.lump.cpp
[ 24/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/9mpy_Win322.lump.cpp
[ 25/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/6ndq_e.Remoting0.lump.cpp
[ 26/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/jmx4_System2.lump.cpp
[ 27/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/0k2r_System0.lump.cpp
[ 28/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/v43o_System0.lump.cpp
[ 29/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/ed5b_System0.lump.cpp
[ 30/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/ed5b_System1.lump.cpp
[ 31/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/0c9e_metadata1.lump.cpp
[ 32/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/edcm_System.Net0.lump.cpp
[ 33/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/xwwj_ThreadPool0.lump.cpp
[ 34/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/n4t3_m.Security0.lump.cpp
[ 35/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/loek_System0.lump.cpp
[ 36/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/9mpy_Win320.lump.cpp
[ 37/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/0c9e_metadata2.lump.cpp
[ 38/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/0c9e_metadata0.lump.cpp
[ 39/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/9mpy_Win323.lump.cpp
[ 40/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/pkfn_os1.lump.cpp
[ 41/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/e7m4_c-api1.lump.cpp
[ 42/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/e7m4_c-api0.lump.cpp
[ 43/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/8f4m_WinRT0.lump.cpp
[ 44/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/1l6r_Android0.lump.cpp
[ 45/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/e7m4_c-api2.lump.cpp
[ 46/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/9mpy_Win324.lump.cpp
[ 46/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/a5ry_OSX0.lump.cpp
[ 48/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kjhk_vm-utils0.lump.cpp
[ 49/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/jmx4_System0.lump.cpp
[ 50/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/2bi8_LibraryPAL0.lump.cpp
[ 51/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/4xol_tests2.lump.cpp
[ 52/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/8f4m_WinRT1.lump.cpp
[ 53/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/xo0x_Generic1.lump.cpp
[ 54/534    0s] WriteText Library/Bee/artifacts/Android/iz17e/_dummy_for_header_discovery
[ 55/534    0s] WriteText Library/Bee/artifacts/Android/87lik/_dummy_for_header_discovery
[ 56/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/qiql_codegen0.lump.cpp
[ 57/534    0s] WriteText Library/Bee/artifacts/Android/dqdht/_dummy_for_header_discovery
[ 58/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/xo0x_Generic2.lump.cpp
[ 59/534    0s] WriteText Library/Bee/artifacts/Android/zz99l/_dummy_for_header_discovery
[ 60/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/uke6_libil2cpp0.lump.cpp
[ 61/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/e7m4_c-api3.lump.cpp
[ 62/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/jmx4_System0.lump.cpp
[ 63/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/4xol_tests3.lump.cpp
[ 64/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/jmx4_System1.lump.cpp
[ 65/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/jmx4_System3.lump.cpp
[ 66/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/t81u_System0.lump.cpp
[ 67/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/y3qr_System0.lump.cpp
[ 68/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/fkj8_Mono0.lump.cpp
[ 69/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/ed5b_System2.lump.cpp
[ 70/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/1t2c_System0.lump.cpp
[ 71/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/jmx4_System2.lump.cpp
[ 72/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/6ndq_e.Remoting0.lump.cpp
[ 73/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/ed5b_System0.lump.cpp
[ 74/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/0c9e_metadata2.lump.cpp
[ 75/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/66pq_em.Runtime0.lump.cpp
[ 76/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/xwwj_ThreadPool0.lump.cpp
[ 77/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/pkfn_os0.lump.cpp
[ 78/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/1l6r_Android0.lump.cpp
[ 79/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/e7m4_c-api0.lump.cpp
[ 80/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/ed5b_System1.lump.cpp
[ 81/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/4xol_tests1.lump.cpp
[ 82/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/e7m4_c-api2.lump.cpp
[ 83/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/pkfn_os1.lump.cpp
[ 84/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/e7m4_c-api1.lump.cpp
[ 85/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/4xol_tests0.lump.cpp
[ 86/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/n4t3_m.Security0.lump.cpp
[ 87/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/td6u_gc0.lump.cpp
[ 88/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/0k2r_System0.lump.cpp
[ 89/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/edcm_System.Net0.lump.cpp
[ 90/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/xo0x_Generic1.lump.cpp
[ 91/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/4xol_tests3.lump.cpp
[ 92/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/a5ry_OSX0.lump.cpp
[ 93/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/zoou_Lumin0.lump.cpp
[ 94/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/hr2q_Posix0.lump.cpp
[ 95/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/fkj8_Mono0.lump.cpp
[ 96/534    0s] WriteText Library/Bee/artifacts/Android/d8kzr/_dummy_for_header_discovery
[ 97/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kzws_utils1.lump.cpp
[ 98/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kzws_utils0.lump.cpp
[ 99/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/2bi8_LibraryPAL0.lump.cpp
[100/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kebh_vm2.lump.cpp
[101/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kebh_vm1.lump.cpp
[102/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/hr2q_Posix0.lump.cpp
[103/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/hr2q_Posix2.lump.cpp
[104/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kebh_vm3.lump.cpp
[105/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/hr2q_Posix1.lump.cpp
[106/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/hr2q_Posix1.lump.cpp
[107/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kzws_utils2.lump.cpp
[108/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/xo0x_Generic2.lump.cpp
[109/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/zoou_Lumin0.lump.cpp
[110/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/hr2q_Posix4.lump.cpp
[111/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/9mpy_Win323.lump.cpp
[112/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/loek_System0.lump.cpp
[113/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/9mpy_Win321.lump.cpp
[114/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/hr2q_Posix2.lump.cpp
[114/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/pkfn_os0.lump.cpp
[114/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kzws_utils2.lump.cpp
[117/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/0c9e_metadata0.lump.cpp
[118/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/v43o_System0.lump.cpp
[114/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/9mpy_Win324.lump.cpp
[120/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kzws_utils1.lump.cpp
[120/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/8f4m_WinRT0.lump.cpp
[122/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kebh_vm0.lump.cpp
[123/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/8f4m_WinRT1.lump.cpp
[124/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kebh_vm1.lump.cpp
[125/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/9mpy_Win322.lump.cpp
[126/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/hr2q_Posix3.lump.cpp
[127/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/psp5_System0.lump.cpp
[128/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kjhk_vm-utils0.lump.cpp
[129/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kebh_vm5.lump.cpp
[130/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kzws_utils0.lump.cpp
[131/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kebh_vm4.lump.cpp
[132/534    0s] MakeLump Library/Bee/artifacts/Android/x6ly2/kebh_vm6.lump.cpp
[133/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kebh_vm6.lump.cpp
[134/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kebh_vm4.lump.cpp
[135/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kebh_vm2.lump.cpp
[136/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kebh_vm5.lump.cpp
[137/534    0s] MakeLump Library/Bee/artifacts/Android/87lik/kebh_vm3.lump.cpp
[BUSY       6s] C_Android_arm32 Library/Bee/artifacts/Android/x6ly2/03dt_stem0.lump.o
[138/534    8s] C_Android_arm32 Library/Bee/artifacts/Android/x6ly2/03dt_stem0.lump.o
[139/534   10s] C_Android_arm64 Library/Bee/artifacts/Android/87lik/907e_tils0.lump.o
[140/534   11s] C_Android_arm64 Library/Bee/artifacts/Android/87lik/z9ad_in322.lump.o
[141/534   13s] C_Android_arm64 Library/Bee/artifacts/Android/87lik/xhlu_inRT1.lump.o
[142/534   16s] C_Android_arm64 Library/Bee/artifacts/Android/87lik/2gjj_h_vm2.lump.o
[143/534   19s] C_Android_arm32 Library/Bee/artifacts/Android/x6ly2/q6z2_tils0.lump.o
[144/534   20s] C_Android_arm64 Library/Bee/artifacts/Android/87lik/8i4j_h_vm4.lump.o
[BUSY      25s] C_Android_arm32 Library/Bee/artifacts/Android/x6ly2/ymgw_h_vm6.lump.o
[145/534   25s] C_Android_arm64 Library/Bee/artifacts/Android/87lik/j5r8_h_vm1.lump.o
[146/534   26s] C_Android_arm64 Library/Bee/artifacts/Android/87lik/ruvu_h_vm5.lump.o
STDERR:
tundra: error: Couldn't launch process with command line:
cmd.exe /c ""C:\Program Files\Unity\Hub\Editor\2021.3.3f1\Editor\Data\il2cpp/build/deploy/UnityLinker.exe" --search-directory="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed" --out="Library/Bee/artifacts/Android/ManagedStripped" --include-link-xml="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed\MethodsToPreserve.xml" --include-link-xml="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed\TypesInScenes.xml" --include-link-xml="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed\SerializedTypes.xml" --include-link-xml="C:\Users\Naviworks_01\Projects\Unity\metaSeoulMain\Assets\AddressableAssetsData\link.xml" --include-link-xml="C:\Users\Naviworks_01\Projects\Unity\metaSeoulMain\Assets\SCM Platform\ThirdParty\Best HTTP\link.xml" --include-link-xml="C:\Users\Naviworks_01\Projects\Unity\metaSeoulMain\Assets\SCM Platform\Plugins\Dissonance\Core\link.xml" --include-link-xml="C:\Users\Naviworks_01\Projects\Unity\metaSeoulMain\Assets\SCM Platform\ThirdParty\LightShaft\YoutubeAPI\link.xml" --include-link-xml="C:\Users\Naviworks_01\Projects\Unity\metaSeoulMain\Assets\SCM Platform\ThirdParty\Photon\PhotonUnityNetworking\link.xml" --include-link-xml="C:\Users\Naviworks_01\Projects\Unity\metaSeoulMain\Assets\SCM Platform\ThirdParty\TriLib\TriLibCore\link.xml" --include-link-xml="C:/Program Files/Unity/Hub/Editor/2021.3.3f1/Editor/Data/PlaybackEngines/AndroidPlayer/Tools/AndroidNativeLink.xml" --include-directory="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed" --dotnetprofile=unityaot-linux --dotnetruntime=Il2Cpp --platform=Android --use-editor-options --engine-modules-asset-file="C:/Program Files/Unity/Hub/Editor/2021.3.3f1/Editor/Data/PlaybackEngines/AndroidPlayer/modules.asset" --editor-data-file="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/EditorToUnityLinkerData.json" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/Assembly-CSharp.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/LeanCommonPlus.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/PhotonChat.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/LeanTouchPlus.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/LeanTouch.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/NativeShare.Runtime.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/Unity.VectorGraphics.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/SoftMask.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/UnityEngine.UI.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/PhotonUnityNetworking.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unity/metaSeoulMain/Temp/StagingArea/Data/Managed/UniTask.TextMeshPro.dll" --include-unity-root-assembly="C:/Users/Naviworks_01/Projects/Unit<message truncated>

===
BuildFailedException: Incremental Player build failed!
UnityEditor.Modules.BeeBuildPostprocessor.PostProcess (UnityEditor.Modules.BuildPostProcessArgs args) (at <31b86d204baf45de8328f2d1261a79f7>:0)
UnityEditor.Modules.DefaultBuildPostprocessor.PostProcess (UnityEditor.Modules.BuildPostProcessArgs args, UnityEditor.BuildProperties& outProperties) (at <31b86d204baf45de8328f2d1261a79f7>:0)
UnityEditor.Android.AndroidBuildPostprocessor.PostProcess (UnityEditor.Modules.BuildPostProcessArgs args, UnityEditor.BuildProperties& outProperties) (at <089eca9a0a77417a901dda0093fda7f3>:0)
UnityEditor.PostprocessBuildPlayer.Postprocess (UnityEditor.BuildTargetGroup targetGroup, UnityEditor.BuildTarget target, System.Int32 subtarget, System.String installPath, System.String companyName, System.String productName, System.Int32 width, System.Int32 height, UnityEditor.BuildOptions options, UnityEditor.RuntimeClassRegistry usedClassRegistry, UnityEditor.Build.Reporting.BuildReport report) (at <31b86d204baf45de8328f2d1261a79f7>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)
