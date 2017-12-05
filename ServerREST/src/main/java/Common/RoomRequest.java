package Common;

import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "RoomRequest")
public class RoomRequest {
	private String title;
	private long Id;
	private boolean createRoom;

	public long getId() {
		return Id;
	}

	public void setId(long id) {
		Id = id;
	}

	public boolean isCreateRoom() {
		return createRoom;
	}

	public void setCreateRoom(boolean createRoom) {
		this.createRoom = createRoom;
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}
}
