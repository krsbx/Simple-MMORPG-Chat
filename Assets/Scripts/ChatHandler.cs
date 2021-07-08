using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using UnityEngine.UIElements;

public class ChatHandler : MonoBehaviour {
  public static ChatHandler Instance;

  public InputField Nicknames;
  public InputField Messages;

  public ScrollRect chatBox;
  public Transform chatHolder;
  public GameObject chatObjects;

  private void Awake () {
    Instance = this;
  }

  public void ValsChanges (object sender, ValueChangedEventArgs task) {
    if (task.Snapshot.HasChildren) {

      foreach (var snap in task.Snapshot.Children) {
        if (chatHolder.childCount > 24) {
          Destroy(chatHolder.GetChild(0).gameObject);
        }

        string result = snap.GetRawJsonValue();
        ChatStructures chats = JsonUtility.FromJson<ChatStructures>(result);

        GameObject chatObj = Instantiate(chatObjects, chatHolder);

        chatObj.transform.Find("Nicknames").GetComponent<Text>().text = chats.Nicknames;
        chatObj.transform.Find("Messages").GetComponent<Text>().text = chats.Messages;

        if (!Nicknames.isFocused && !Messages.isFocused) {
          Canvas.ForceUpdateCanvases();

          chatBox.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
          chatBox.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();

          chatBox.verticalNormalizedPosition = 0;
        }
      }
    }
  }

  public async void SendMessages () {
    if (string.IsNullOrEmpty(Nicknames.text) && string.IsNullOrEmpty(Messages.text)) return;

    ChatStructures chats = new ChatStructures(Nicknames.text, Messages.text);
    string result = JsonUtility.ToJson(chats);

    await FirebaseInitializer.ChatReferences.Push().SetRawJsonValueAsync(result);
  }
}
