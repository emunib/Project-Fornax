/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package client;

import Common.ConversationInfo;
import Common.Message;

import java.util.*;

/**
 *
 * @author nathanhupka
 */
class ClientConversation {
    String title;
    String privateId;
    String publicId;
    LinkedList<MessageHolder> messageList;
    Chatroom associatedChatroom;

    ClientConversation(String newPrivateId, String newPublicId, String ntitle){
        title = ntitle;
        privateId = newPrivateId;
        publicId = newPublicId;
        messageList = new LinkedList<>();
    }
    
    void SetRoom(Chatroom nAssociatedChatroom){
        associatedChatroom = nAssociatedChatroom;
    }

    void update(List<Message> nMessageList){
        for (Message msg:
             nMessageList) {
            messageList.addLast(new MessageHolder(msg));
        }
        associatedChatroom.list.setListData(messageList.toArray());
    }
}

class ClientConversationList extends HashMap<Long, ConversationInfo> {
    public LocalConversationInfo[] toArray() {
        ArrayList<LocalConversationInfo> list = new ArrayList<>();
        super.forEach((Long aLong, ConversationInfo conv) -> {
            list.add(new LocalConversationInfo(conv));
        });
        return list.toArray(new LocalConversationInfo[]{});
    }
}

public class ClientData {
    static Client2Server client2server;
    static ClientConversationList conversations;
    static final Map<String, ClientConversation> activeConvMap = new HashMap<>();
    static MainWindow mainWindow;
}
