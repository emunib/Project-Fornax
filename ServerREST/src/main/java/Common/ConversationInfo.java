package Common;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "ConversationInfo")
public class ConversationInfo {
	private long id;
	private String title;
	private long messageCount;

	public long getId() {
		return id;
	}

	public void setId(long id) {
		this.id = id;
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	public long getMessageCount() {
		return messageCount;
	}

	public void setMessageCount(long messageCount) {
		this.messageCount = messageCount;
	}
}
