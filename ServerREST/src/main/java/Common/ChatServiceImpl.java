/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Common;

import javax.ws.rs.core.Response;
import java.time.Clock;
import java.time.Instant;
import java.time.ZoneId;
import java.time.format.DateTimeFormatter;
import java.time.temporal.ChronoUnit;
import java.util.*;

public class ChatServiceImpl implements ChatService {

    final public static Map<String, Session> sessions = new HashMap<>();
    final public Map<Long, Conversation> conversations = new HashMap<>();
    final public Map<String, User> userMap = new HashMap<>();
    //final public Map<PlayerImpl> activerUsers = new HashSet<>();
    final static public Clock clock = Clock.systemUTC();
    final public static Random rand = new Random(Clock.systemUTC().millis());
    public long conversationCount = 0;

    public ChatServiceImpl() {

    }

    public static String getTimeStamp(){
        Instant temp = clock.instant();
        StringBuilder stime = new StringBuilder();
        stime.append(DateTimeFormatter.ISO_DATE.withZone(ZoneId.of("UTC")).format(temp)).append(":");
        stime.append(DateTimeFormatter.ISO_TIME.withZone(ZoneId.of("UTC")).format(temp.truncatedTo(ChronoUnit.SECONDS)));
        String time = stime.toString().replace("-",":");
        String[] timearray = time.split("Z");
        stime = new StringBuilder();
        for(int i = 0; i < timearray.length; i++){
            stime.append(timearray[i]);
        }
        return stime.toString();
    }

    public String createUserCommand(String user, String password){
        User temp = userMap.get(user);
        if (temp == null){
                User newUser = new User(user,password);
                userMap.put(user, newUser);
                //activerUsers.add(newUser);
                Session newsession = new Session(newUser, this);
                sessions.put(newsession.getId(), newsession);
                return newsession.getId();
        } else {
            return "";
        }
    }

    @Override
    public String login(User user) {
            String username = user.Username;
            String password = user.Password;
            User temp = userMap.get(username);
            if (temp != null) {
                if (password.equals(password)) { /* && !activerUsers.contains(temp)){ */
                    //activerUsers.add(temp);
                    Session newsession = new Session(temp, this);
                    sessions.put(newsession.getId(), newsession);
                    return  newsession.getId();
                }
                return "";
            } else {
                return createUserCommand(username, password);
            }
    }

    @Override
    public Response logout(String sessionId) {
        if (sessions.containsKey(sessionId)) {
            Session session = sessions.get(sessionId);
            session.getMyConversations().forEach((String s, ConvInterface convInterface) -> {
                convInterface.getConv().getUsers().remove(session.getUser().Username);
            });
            session.getMyConversations().clear();
            sessions.remove(sessionId);
        }
        return Response.ok().build();
    }

    @Override
    public ConversationInfoList getconversations() {
            List<ConversationInfo> list = new LinkedList<>();
            conversations.forEach((Long k, Conversation v) -> {
               ConversationInfo convInfo = new ConversationInfo();
               convInfo.setId(v.getId());
               convInfo.setTitle(v.getTitle());
               convInfo.setMessageCount(v.getMessageList().size());
               list.add(convInfo);
            });
        ConversationInfoList conversationInfoList = new ConversationInfoList();
        conversationInfoList.setList(list);
        return conversationInfoList;
    }

    @Override
    public Session getSession(String sessionId) {
        return sessions.get(sessionId);
    }

    @Override
    public ConversationInfo getconversation(String id) {
        Conversation conv = conversations.get(Long.parseLong(id));
        ConversationInfo conversationInfo = new ConversationInfo();
        conversationInfo.setMessageCount(conv.getMessageList().size());
        conversationInfo.setTitle(conv.getTitle());
        conversationInfo.setId(conv.getId());
        return conversationInfo;
    }

    @Override
    public Message getmessage(String convId, String messageId) {
            Conversation conversation = conversations.get(Long.parseLong(convId));
            if (conversation != null){
                return conversation.getMessageList().get(messageId);
            }
            return null;
    }

    class ActivityChecker implements Runnable {

        @Override
        public void run() {

        }
    }

    public void getUsersCommand() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }
    
}
