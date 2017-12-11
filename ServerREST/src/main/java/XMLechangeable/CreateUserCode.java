package XMLechangeable;

import javax.xml.bind.annotation.*;

@XmlType(name = "CreateUserCode")
public enum CreateUserCode {

	@XmlEnumValue("SUCCESS") SUCCESS,
	@XmlEnumValue("INVALIDUSRNM") INVALIDUSRNM,
	@XmlEnumValue("ALREADYEXISTS") ALREADYEXISTS,
}

