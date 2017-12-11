package njh;

import XMLechangeable.SessionInfo;

public class SessionImpl extends SessionInfo {
	final PlayerImpl player;
	long lastTouched;

	public SessionImpl(PlayerImpl nplayer, String publicId, String privateID){
		player = nplayer;
		setUsername(player.getUsername());
		setPublicID(publicId);
		setPrivateID(privateID);
		lastTouched = 0;
	}

	public boolean isValidSession(SessionInfo session) {
		lastTouched = GlobalClock.GetTime();
		return getPrivateID().equals(session.getPrivateID());
	}

	public boolean isValidSession(String privateID) {
		lastTouched = GlobalClock.GetTime();
		return getPrivateID().equals(privateID);
	}

	public boolean isActive(){
		return (GlobalClock.GetTime() - lastTouched < 10);
	}
}
