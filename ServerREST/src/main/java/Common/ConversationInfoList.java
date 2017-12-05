package Common;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.List;

@XmlRootElement(name = "ConversationInfoList")
public class ConversationInfoList {
	private List<ConversationInfo> List;

	public java.util.List<ConversationInfo> getList() {
		return List;
	}

	public void setList(java.util.List<ConversationInfo> list) {
		List = list;
	}

}
