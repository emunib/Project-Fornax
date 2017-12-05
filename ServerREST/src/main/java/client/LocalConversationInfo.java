package client;

import Common.ConversationInfo;

public class LocalConversationInfo {
    public ConversationInfo info;
    LocalConversationInfo(ConversationInfo ninfo){
        info = ninfo;
    }

    @Override
    public String toString() {
        return Long.toString(info.getId())+ ":" + info.getTitle();
    }
}
