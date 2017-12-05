/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package client;

import javax.swing.*;
import java.io.IOException;
import java.net.URL;

/**
 *
 * @author nathanhupka
 */
public class Client {
    /**
     * @param args the command line arguments
     * @throws IOException
     */
    public static void main(String[] args) throws IOException {
        ClientData.client2server = new Client2Server();
        JFrame frame = new Login();
        frame.setVisible(true);
    }
    
    public static void NewMainWindow(){
        
    }
 
    }
