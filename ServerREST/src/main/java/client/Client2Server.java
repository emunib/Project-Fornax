/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package client;

import Common.*;

import javax.ws.rs.client.Client;
import javax.ws.rs.client.ClientBuilder;
import javax.ws.rs.client.Entity;
import javax.ws.rs.client.WebTarget;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import java.util.ArrayList;
import java.util.LinkedList;

/**
 *
 * @author nathanhupka
 */

public class Client2Server {
    final static String CHATSERVICE_URL = "http://localhost:8080/services/chatservice";
    final static String CONVERSATIONS_URL = "/conversations";
    final static String SESSIONS_URL = "/sessions";
    final static String MESSAGE_URL = "/messages";
    Client client;
    String sessionId;
    Client2Server(){
        Thread thread = new Thread(new Mailman());
        thread.start();
        client = ClientBuilder.newClient();
    }
             
    boolean login(String username, String password){
        WebTarget target = client.target(CHATSERVICE_URL).path(SESSIONS_URL);
        User user = new User(username, password);
        sessionId = target.request().post(Entity.entity(user, MediaType.APPLICATION_XML), String.class);
        return (!sessionId.equals(""));
    }
    
    boolean createUser(String username, String password){
        WebTarget target = client.target(CHATSERVICE_URL).path(SESSIONS_URL);
        User user = new User(username, password);
        sessionId = target.request().post(Entity.entity(user, MediaType.APPLICATION_XML), String.class);
        return (!sessionId.equals(""));
    }
    
    void getconversationsCommand(){
        WebTarget target = client.target(CHATSERVICE_URL).path(CONVERSATIONS_URL);
        ConversationInfoList clientConversations = target.request().get(ConversationInfoList.class);
        ClientData.conversations = new ClientConversationList();
        if (clientConversations.getList() != null) {
            for (ConversationInfo conv : clientConversations.getList()) {
                ClientData.conversations.put(conv.getId(), conv);
            }
        }
    }
    
    ClientConversation createconversationCommand(String name){
        WebTarget target = client.target(CHATSERVICE_URL).path(SESSIONS_URL + "/" + sessionId).path(CONVERSATIONS_URL);
        RoomRequest roomRequest = new RoomRequest();
        roomRequest.setTitle(name);
        roomRequest.setCreateRoom(true);
        RoomResponse response = target.request().post(Entity.entity(roomRequest, MediaType.APPLICATION_XML), RoomResponse.class);
        if (response.getPrivateId() != ""){
            ClientConversation nConv = new ClientConversation(response.getPrivateId(), response.getPublicId(), name);
            synchronized (ClientData.activeConvMap) {
                ClientData.activeConvMap.put(nConv.privateId, nConv);
            }
           return nConv;
       } 
       return null;
    }
    
    void logoutCommand(){
        WebTarget target = client.target(CHATSERVICE_URL).path(SESSIONS_URL + "/" + sessionId);
        Response delete = target.request().delete();
        synchronized (ClientData.activeConvMap){
            ClientData.activeConvMap.clear();
        }
        sessionId = "";
    }

    ClientConversation joinconversationCommand(long id){
        WebTarget target = client.target(CHATSERVICE_URL).path(SESSIONS_URL + "/" + sessionId).path(CONVERSATIONS_URL);;
        RoomRequest roomRequest = new RoomRequest();
        roomRequest.setId(id);
        roomRequest.setCreateRoom(false);
        RoomResponse response = target.request().post(Entity.entity(roomRequest, MediaType.APPLICATION_XML), RoomResponse.class);
        if (response.getPrivateId() != ""){
            ClientConversation nConv = new ClientConversation(response.getPrivateId(), response.getPublicId(), ClientData.conversations.get(id).getTitle());
            synchronized (ClientData.activeConvMap) {
                ClientData.activeConvMap.put(response.getPrivateId(), nConv);
            }
            return nConv;
        } 
        return null;
    }
    
    boolean leaveconversationCommand(ClientConversation conv){
        WebTarget target = client.target(CHATSERVICE_URL).path(SESSIONS_URL + "/" + sessionId).path(CONVERSATIONS_URL + "/" + conv.privateId);
        Response response = target.request().delete();
        if (response.getStatus() == Response.ok().build().getStatus()){
            synchronized (ClientData.activeConvMap) {
                ClientData.activeConvMap.remove(conv.privateId);
            }
            conv.messageList.clear();
            return true;
        }
        return false;
    }
    
    boolean sendmessage(ClientConversation conv, String message){
        WebTarget target = client.target(CHATSERVICE_URL).path(SESSIONS_URL + "/" + sessionId).path(CONVERSATIONS_URL + "/" + conv.privateId).path(MESSAGE_URL);
        Message message1 = new Message();
        message1.setContent(message);
        Response response = target.request().post(Entity.entity(message1, MediaType.APPLICATION_XML));
        return (response.getStatus() == Response.ok().build().getStatus());
    }

    class Mailman implements Runnable {
        @Override
        public void run() {
            while (true){
                synchronized (ClientData.activeConvMap){
                    ClientData.activeConvMap.forEach((String convId, ClientConversation clientConversation) -> {
                        WebTarget target = client.target(CHATSERVICE_URL).path(CONVERSATIONS_URL + "/" + clientConversation.publicId);
                        ConversationInfo conversationInfo = target.request().get(ConversationInfo.class);
                        if (clientConversation.messageList.size() < conversationInfo.getMessageCount()){
                            long count = clientConversation.messageList.size();
                            LinkedList<Message> list = new LinkedList<>();
                            while (list.size() < conversationInfo.getMessageCount() - clientConversation.messageList.size()){
                                count++;
                                target = client.target(CHATSERVICE_URL).path(CONVERSATIONS_URL + "/" + clientConversation.publicId).path(MESSAGE_URL + "/" + Int2AlphaNumeric.Convert(count));
                                Message message = target.request().get(Message.class);
                                list.addLast(message);
                            }
                            clientConversation.update(list);
                        }
                    });
                }
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }
}
