#if(UNITY_EDITOR)
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class SimpleEditorUtils {
    // click command-0 to go to the prelaunch scene and then play

    [MenuItem("Edit/Play From Launch Scene %0")]
    public static void PlayFromPrelaunchScene() {
        if (EditorApplication.isPlaying == true) {
            EditorApplication.isPlaying = false;
            return;
        }
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(
                    "Assets/Scenes/SceneController.unity");
        EditorApplication.isPlaying = true;
    }
}
#endif