package Common;

import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "RoomRequest")
public class RoomResponse {
	private String privateId;
	private String publicId;
	private String title;

	public String getPrivateId() {
		return privateId;
	}

	public void setPrivateId(String privateId) {
		this.privateId = privateId;
	}

	public String getPublicId() {
		return publicId;
	}

	public void setPublicId(String publicId) {
		this.publicId = publicId;
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}
}
