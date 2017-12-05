package Common;

import javax.ws.rs.*;
import javax.ws.rs.core.Response;
import java.util.List;

@Produces({ "application/xml", "application/json" })
public interface ChatService {

	@POST
	@Path("/sessions")
	String login(User user);

	@DELETE
	@Path("/sessions/{id}")
	Response logout(@PathParam("id")String id);

	@GET
	@Path("/conversations")
	ConversationInfoList getconversations();

	@Path("/sessions/{id}")
	Session getSession(@PathParam("id") String sessionId);

	@GET
	@Path("/conversations/{id}")
	ConversationInfo getconversation(@PathParam("id") String id);

	@GET
	@Path("/conversations/{id}/messages/{messageId}")
	Message getmessage(@PathParam("id") String convId, @PathParam("messageId") String messageId);

}