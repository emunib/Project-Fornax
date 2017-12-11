package XMLechangeable;

import javax.xml.bind.annotation.XmlEnumValue;
import javax.xml.bind.annotation.XmlType;

@XmlType(name = "LoginCode")
public enum LoginCode {

	@XmlEnumValue("SUCCESS") SUCCESS,
	@XmlEnumValue("ALREADYACTIVE") ALREADYACTIVE,
	@XmlEnumValue("DOESNTEXIST") DOESNTEXIST,
	@XmlEnumValue("BADPASSWORD") BADPASSWORD,
}