package Common;

import javax.ws.rs.*;
import javax.ws.rs.core.Response;
import java.util.HashMap;

/**
 *
 * @author nathanhupka
 */

public class Session {
    private User user;
    private String Id;
    private HashMap<String, ConvInterface> myConversations;
    private IdGenerator idGenerator = new IdGenerator();
    private ChatServiceImpl chatServicePort;

    public User getUser() {
        return user;
    }

    public String getId() {
        return Id;
    }


    public HashMap<String, ConvInterface> getMyConversations() {
        return myConversations;
    }

    public RoomResponse createconversation(RoomRequest request){
        Conversation newConversation = new Conversation(request.getTitle(), chatServicePort);
        newConversation.getUsers().put(user.Username, user);

        ConvInterface convInterface = new ConvInterface(user, newConversation);
        String convId = idGenerator.GetId();
        myConversations.put(convId, convInterface);

        chatServicePort.conversations.put(newConversation.getId(), newConversation);
        RoomResponse result = new RoomResponse();
        result.setTitle(request.getTitle());
        result.setPublicId(newConversation.getId().toString());
        result.setPrivateId(convId);
        return result;
    }

    @POST
    @Path("/conversations")
    public RoomResponse joinconversation(RoomRequest request){
        if (request.isCreateRoom()){
            return createconversation(request);
        }
        Conversation conv = chatServicePort.conversations.get(request.getId());
        conv.getUsers().put(user.Username, user);

        ConvInterface convInterface = new ConvInterface(user, conv);


        String convId = idGenerator.GetId();
        myConversations.put(convId, convInterface);

        RoomResponse result = new RoomResponse();
        result.setTitle(conv.getTitle());
        result.setPublicId(conv.getId().toString());
        result.setPrivateId(convId);
        return result;
    }

    @Path("/conversations/{id}")
    public ConvInterface getconversation(@PathParam("id") String id){
        return myConversations.get(id);
    }

    @DELETE
    @Path("/conversations/{id}")
    public Response leaveconversation(@PathParam("id") String id){
        Response response = null;
        ConvInterface convInterface = myConversations.get(id);
        if (convInterface != null){
            Conversation conv = convInterface.getConv();
            conv.getUsers().remove(user.Username);

            myConversations.remove(id);
            idGenerator.ReturnId(id);
            response = Response.ok().build();
        } else {
            response = Response.notModified().build();
        }
        return response;
    }


    Session(User nuser, ChatServiceImpl server){
        chatServicePort = server;
        myConversations = new HashMap<>();
        user = nuser;
        String id = GenId();
        while (ChatServiceImpl.sessions.containsKey(id)){
            id = GenId();
        }
        Id = GenId();
    }

    private String GenId(){
        String result = "";
        result += ChatServiceImpl.rand.nextLong();
        return result;
    }
}
