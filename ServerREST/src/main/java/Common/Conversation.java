
package Common;

import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.core.Response;
import java.util.HashMap;


public class Conversation {
    private Long id;
    private String title;
    private HashMap<String, User> users;
    private HashMap<String, Message> messageList;
    private ChatServiceImpl Server;
    private IdGenerator idGenerator;

    public Long getId() {
        return id;
    }

    public String getTitle() {
        return title;
    }

    public HashMap<String, User> getUsers() {
        return users;
    }

    public HashMap<String, Message> getMessageList() {
        return messageList;
    }

    Conversation(String nTitle, ChatServiceImpl nServer){
        title = nTitle;
        Server = nServer;
        id = Server.conversationCount++ + 1;
        users = new HashMap<>();
        messageList = new HashMap<>();
        idGenerator = new IdGenerator();
    }

    public void addMessage(Message newmessage){
        messageList.put(idGenerator.GetId(), newmessage);
    }
}
