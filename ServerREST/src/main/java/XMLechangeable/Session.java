package XMLechangeable;

import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "Session")
public class Session {
	private String publicID;
	private String privateID;
	private String username;

	public String getPublicID() {
		return publicID;
	}

	public void setPublicID(String publicID) {
		this.publicID = publicID;
	}

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
