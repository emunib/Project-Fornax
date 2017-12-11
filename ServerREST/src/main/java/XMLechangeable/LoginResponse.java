package XMLechangeable;

import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;

@XmlRootElement(name = "LoginResponse")
public class LoginResponse {
	private LoginCode response;
	private SessionInfo sessionInfo;

	public LoginCode getResponse() {
		return response;
	}

	public void setResponse(LoginCode response) {
		this.response = response;
	}

	public SessionInfo getSessionInfo() {
		return sessionInfo;
	}

	public void setSessionInfo(SessionInfo sessionInfo) {
		this.sessionInfo = sessionInfo;
	}
}
