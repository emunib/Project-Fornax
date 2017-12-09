package XMLechangeable;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.List;

@XmlRootElement(name = "PublicGameInfo")
public class PublicGameInfo {
	String gameID;
	UserPublic Host;
	List<UserPublic> Players;
	boolean isLocked;

	public String getGameID() {
		return gameID;
	}

	public void setGameID(String gameID) {
		this.gameID = gameID;
	}

	public UserPublic getHost() {
		return Host;
	}

	public void setHost(UserPublic host) {
		Host = host;
	}

	public List<UserPublic> getPlayers() {
		return Players;
	}

	public void setPlayers(List<UserPublic> players) {
		Players = players;
	}

	public boolean isLocked() {
		return isLocked;
	}

	public void setLocked(boolean locked) {
		isLocked = locked;
	}
}
