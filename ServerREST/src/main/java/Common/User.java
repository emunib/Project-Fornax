/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Common;

import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author nathanhupka
 */

@XmlRootElement(name = "User")
public class User {
    String Username;
    String Password;

    public String getUsername() {
        return Username;
    }

    public void setUsername(String username) {
        Username = username;
    }

    public String getPassword() {
        return Password;
    }

    public void setPassword(String password) {
        Password = password;
    }

    public User(){

    }
    public User(String username, String password){
        Username = username;
        Password = password;
    }
}
