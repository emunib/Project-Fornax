package XMLechangeable;

import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "CreateUserResponse")
public class CreateUserResponse {
	private CreateUserCode response;
	private SessionInfo sessionInfo;

	public SessionInfo getSessionInfo() {
		return sessionInfo;
	}

	public void setSessionInfo(SessionInfo sessionInfo) {
		this.sessionInfo = sessionInfo;
	}

	public CreateUserCode getResponse() {
		return response;
	}

	public void setResponse(CreateUserCode response) {
		this.response = response;
	}
}
