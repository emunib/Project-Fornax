/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package client;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;

/**
 *
 * @author nathanhupka
 */
public class Chatroom extends JPanel {
    JScrollPane listView;
    JList list;
    JTextField field;
    JButton button;
    JTabbedPane parent;
    ClientConversation conv;
    Chatroom(JTabbedPane nParent, ClientConversation nConv){
        conv = nConv;
        conv.SetRoom(this);
        parent = nParent;
        list = new JList();
        field = new JTextField();
        button = new JButton();
        listView = new JScrollPane(list);
        listView.setHorizontalScrollBarPolicy(ScrollPaneConstants.HORIZONTAL_SCROLLBAR_NEVER);
        button.setText("Leave");
        GridBagLayout layout = new GridBagLayout();
        GridBagConstraints gbc = new GridBagConstraints();
        
        //gbc.anchor = GridBagConstraints.SOUTH;
        this.setLayout(layout);
        gbc.anchor = GridBagConstraints.EAST;
        gbc.gridy = 0;
        gbc.weightx = gbc.weighty = 0;
        gbc.fill = GridBagConstraints.NONE;
        this.add(button, gbc);
        gbc.gridy = 1;
        gbc.anchor = GridBagConstraints.CENTER;
        gbc.weightx = gbc.weighty = 1;
        gbc.fill = GridBagConstraints.BOTH;
        this.add(listView, gbc);
        gbc.gridy = 2;
        gbc.weighty = 0;
        gbc.fill = GridBagConstraints.HORIZONTAL;
        this.add(field, gbc);
        field.addKeyListener(new KeyAdapter() {
            @Override
            public void keyPressed(KeyEvent e) {
                if(e.getKeyCode() == KeyEvent.VK_ENTER){
                    String message = field.getText();
                    if(!message.isEmpty()){
                       ClientData.client2server.sendmessage(conv, message);
                       field.setText(null);
                    }
                }
            }
        });
        button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if (ClientData.client2server.leaveconversationCommand(conv)){
                    parent.remove(parent.getSelectedComponent());
                } else {
                    field.setText("Bad Command");
                }
            }
            }
        );
    }
}
