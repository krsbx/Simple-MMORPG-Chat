using UnityEngine;
using Firebase.Database;

public class FirebaseInitializer : MonoBehaviour {
  public static DatabaseReference ChatReferences;
  private void Start () {
    ChatReferences = FirebaseDatabase.DefaultInstance.GetReference("MMORPG");

    ChatReferences.LimitToLast(25).ValueChanged += ChatHandler.Instance.ValsChanges;
  }
}
