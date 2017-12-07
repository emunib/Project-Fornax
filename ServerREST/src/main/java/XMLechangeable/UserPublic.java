package XMLechangeable;

import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "UserPublic")
public class UserPublic {
	private String Username;

	public String getUsername() {
		return Username;
	}

	public void setUsername(String username) {
		Username = username;
	}
}
