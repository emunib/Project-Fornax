package XMLechangeable;

import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "SessionInfo")
public class SessionInfo extends PublicSessionInfo {
	private String privateID;
	private String username;

	public String getPrivateID() {
		return privateID;
	}

	public void setPrivateID(String privateID) {
		this.privateID = privateID;
	}

	public String getUsername() {
		return username;
	}

	public void setUsername(String username) {
		this.username = username;
	}
}
