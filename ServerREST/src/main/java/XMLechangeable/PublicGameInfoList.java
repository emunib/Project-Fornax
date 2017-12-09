package XMLechangeable;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.List;

@XmlRootElement(name = "PublicGameInfoList")
public class PublicGameInfoList {
	private List<PublicGameInfo> publicGameInfos;

	public List<PublicGameInfo> getPublicGameInfos() {
		return publicGameInfos;
	}

	public void setPublicGameInfos(List<PublicGameInfo> publicGameInfos) {
		this.publicGameInfos = publicGameInfos;
	}
}
