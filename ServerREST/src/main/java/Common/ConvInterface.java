package Common;

import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.core.Response;

/**
 * <p>Java class for conversation complex type.
 *
 * <p>The following schema fragment specifies the expected content contained within this class.
 *
 * <pre>
 * &lt;complexType name="conversation"&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;all&gt;
 *         &lt;element name="id" type="{http://www.w3.org/2001/XMLSchema}long"/&gt;
 *         &lt;element name="title" type="{http://www.w3.org/2001/XMLSchema}string"/&gt;
 *       &lt;/all&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 *
 *
 */
public class ConvInterface {
    private Conversation conv;
    private String username;

    public Conversation getConv() {
        return conv;
    }

    ConvInterface(User user, Conversation nconv) {
        conv = nconv;
        username = user.Username;
    }

    @POST
    @Path("/messages")
    public Response sendmessage(Message msg){
        Message newmessage = new Message();
        newmessage.setContent(msg.getContent());
        newmessage.setTime(ChatServiceImpl.getTimeStamp());
        newmessage.setUser(username);
        conv.addMessage(newmessage);
        return Response.ok().build();
    }
}
