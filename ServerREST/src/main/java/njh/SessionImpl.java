package njh;

import XMLechangeable.SessionInfo;

public class SessionImpl extends SessionInfo {
	final PlayerImpl player;

	public SessionImpl(PlayerImpl nplayer, String publicId, String privateID){
		player = nplayer;
		setUsername(player.getUsername());
		setPublicID(publicId);
		setPrivateID(privateID);
	}

	public boolean isValidSession(SessionInfo session) {
		return getPrivateID().equals(session.getPrivateID());
	}

	public boolean isValidSession(String privateID) {
		return getPrivateID().equals(privateID);
	}
}
