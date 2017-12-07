/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package client;

import com.sun.glass.events.KeyEvent;

import java.time.Clock;
import java.time.Instant;
import java.time.ZoneId;
import java.time.format.DateTimeFormatter;
import java.time.temporal.ChronoUnit;
import java.util.Timer;
import java.util.TimerTask;

/**
 *
 * @author nathanhupka
 */
public class MainWindow extends javax.swing.JFrame {
    final private Timer timer;
    
    class ClockUpdater extends TimerTask{
        Clock clock;
        public ClockUpdater(){
            clock = Clock.systemUTC();
        }
        
        public void run(){
            Instant temp = clock.instant();
            StringBuilder stime = new StringBuilder();
            stime.append(DateTimeFormatter.ISO_DATE.withZone(ZoneId.of("UTC")).format(temp)).append(":");
            stime.append(DateTimeFormatter.ISO_TIME.withZone(ZoneId.of("UTC")).format(temp.truncatedTo(ChronoUnit.SECONDS)));
            String[] timearray = stime.toString().split("Z");
            stime = new StringBuilder();
            for(int i = 0; i < timearray.length; i++){
                stime.append(timearray[i]);
            }
            TimeLabel.setText(stime.toString());
        }
    }
    
    static public void NewMainWindow(String username){ 
                ClientData.mainWindow = new MainWindow(username);;
                ClientData.mainWindow.setVisible(true);
    }
   
    public void DestroyMainWindow(){
            ClientData.mainWindow = null;
            this.dispose();
    }
    
    /**
     * Creates new form MainWindow
     * @param username
     */
    private MainWindow(String username) {
        initComponents();
        timer = new Timer();
        timer.schedule(new ClockUpdater(), 0, 1000);
        Username.setText("PlayerImpl: " + username);
        FetchnRefreshConvLst();
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jPanel2 = new javax.swing.JPanel();
        NChtRmNmField = new javax.swing.JTextField();
        CreateChatRoomButton = new javax.swing.JButton();
        CancelButton = new javax.swing.JButton();
        QuitButton = new javax.swing.JButton();
        LogoutButton = new javax.swing.JButton();
        jTabbedPane1 = new javax.swing.JTabbedPane();
        jPanel1 = new javax.swing.JPanel();
        jScrollPane1 = new javax.swing.JScrollPane();
        AvlChtRmList = new javax.swing.JList<>();
        CreateButton = new javax.swing.JButton();
        JoinButton = new javax.swing.JButton();
        RefreshButton = new javax.swing.JButton();
        Username = new javax.swing.JLabel();
        TimeLabel = new javax.swing.JLabel();

        NChtRmNmField.setText("Chatroom Name");
        NChtRmNmField.addKeyListener(new java.awt.event.KeyAdapter() {
            public void keyPressed(java.awt.event.KeyEvent evt) {
                NChtRmNmFieldKeyPressed(evt);
            }
        });

        CreateChatRoomButton.setText("Create");
        CreateChatRoomButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                CreateChatRoomButtonActionPerformed(evt);
            }
        });

        CancelButton.setText("Cancel");
        CancelButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                CancelButtonActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel2Layout = new javax.swing.GroupLayout(jPanel2);
        jPanel2.setLayout(jPanel2Layout);
        jPanel2Layout.setHorizontalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel2Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(NChtRmNmField)
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel2Layout.createSequentialGroup()
                        .addGap(0, 73, Short.MAX_VALUE)
                        .addComponent(CancelButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(CreateChatRoomButton)))
                .addContainerGap())
        );
        jPanel2Layout.setVerticalGroup(
            jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel2Layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(NChtRmNmField, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel2Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(CreateChatRoomButton)
                    .addComponent(CancelButton))
                .addContainerGap(122, Short.MAX_VALUE))
        );

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);

        QuitButton.setText("Quit");
        QuitButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                QuitButtonActionPerformed(evt);
            }
        });

        LogoutButton.setText("Logout");
        LogoutButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                LogoutButtonActionPerformed(evt);
            }
        });

        AvlChtRmList.setModel(new javax.swing.AbstractListModel<LocalConversationInfo>() {
            LocalConversationInfo[] strings = {};
            public int getSize() { return strings.length; }
            public LocalConversationInfo getElementAt(int i) { return strings[i]; }
        });
        jScrollPane1.setViewportView(AvlChtRmList);

        CreateButton.setText("Create");
        CreateButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                CreateButtonActionPerformed(evt);
            }
        });

        JoinButton.setText("Join");
        JoinButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                JoinButtonActionPerformed(evt);
            }
        });

        RefreshButton.setText("Refresh");
        RefreshButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                RefreshButtonActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 211, Short.MAX_VALUE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addComponent(JoinButton, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addComponent(CreateButton, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addComponent(RefreshButton, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                .addContainerGap())
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addComponent(CreateButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(JoinButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(RefreshButton)
                        .addGap(0, 0, Short.MAX_VALUE))
                    .addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 179, Short.MAX_VALUE))
                .addContainerGap())
        );

        jTabbedPane1.addTab("Join or Create Chatroom", jPanel1);

        Username.setText("jLabel1");

        TimeLabel.setText("jLabel2");

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jTabbedPane1)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGap(0, 0, Short.MAX_VALUE)
                        .addComponent(QuitButton)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(LogoutButton))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(Username)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(TimeLabel)))
                .addContainerGap())
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(Username)
                    .addComponent(TimeLabel))
                .addGap(3, 3, 3)
                .addComponent(jTabbedPane1)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(QuitButton)
                    .addComponent(LogoutButton))
                .addContainerGap())
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void FetchnRefreshConvLst(){
        ClientData.client2server.getconversationsCommand();
        AvlChtRmList.setListData(ClientData.conversations.toArray());
    }
    
    public void RefreshConvLst(){
        AvlChtRmList.setListData(ClientData.conversations.toArray());
    }
    
    private void QuitButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_QuitButtonActionPerformed
        ClientData.client2server.logoutCommand();
        System.exit(0);
    }//GEN-LAST:event_QuitButtonActionPerformed

    private void CancelButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_CancelButtonActionPerformed
        jTabbedPane1.remove(jTabbedPane1.getSelectedIndex());
    }//GEN-LAST:event_CancelButtonActionPerformed

    private void JoinButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_JoinButtonActionPerformed
        LocalConversationInfo conv = AvlChtRmList.getSelectedValue();
        if (conv != null){
            ClientConversation clientConversation = ClientData.client2server.joinconversationCommand(conv.info.getId());
            if (clientConversation != null){
                jTabbedPane1.setComponentAt(jTabbedPane1.getSelectedIndex(), new Chatroom(jTabbedPane1, clientConversation));
                jTabbedPane1.setTitleAt(jTabbedPane1.getSelectedIndex(), clientConversation.title);
                jTabbedPane1.addTab("Join or Create Chatroom", jPanel1);
            }
        }
    }//GEN-LAST:event_JoinButtonActionPerformed

    private void CreateButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_CreateButtonActionPerformed
        jTabbedPane1.setComponentAt(jTabbedPane1.getSelectedIndex(), jPanel2);
        jTabbedPane1.setTitleAt(jTabbedPane1.getSelectedIndex(), "New room");
         jTabbedPane1.addTab("Join or Create Chatroom", jPanel1);
    }//GEN-LAST:event_CreateButtonActionPerformed

    private void CreateChatRoom(){
        String name = NChtRmNmField.getText();
        if (name.isEmpty()){
            NChtRmNmField.setText("Can not be empty");
        } else {
            ClientConversation nConv;
            if ((nConv = ClientData.client2server.createconversationCommand(name)) != null){
                jTabbedPane1.setComponentAt(jTabbedPane1.getSelectedIndex(), new Chatroom(jTabbedPane1, nConv));
                jTabbedPane1.setTitleAt(jTabbedPane1.getSelectedIndex(), name);
            } else {
                NChtRmNmField.setText("Bad Command");
            }
        }
    }
    private void CreateChatRoomButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_CreateChatRoomButtonActionPerformed
        CreateChatRoom();
    }//GEN-LAST:event_CreateChatRoomButtonActionPerformed

    private void LogoutButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_LogoutButtonActionPerformed
        ClientData.client2server.logoutCommand();
        Login window = new Login();
        window.setVisible(true);
        DestroyMainWindow();
    }//GEN-LAST:event_LogoutButtonActionPerformed

    private void NChtRmNmFieldKeyPressed(java.awt.event.KeyEvent evt) {//GEN-FIRST:event_NChtRmNmFieldKeyPressed
        if (evt.getKeyCode() == KeyEvent.VK_ENTER){
            CreateChatRoom();
        }
    }//GEN-LAST:event_NChtRmNmFieldKeyPressed

    private void RefreshButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_RefreshButtonActionPerformed
        FetchnRefreshConvLst();
    }//GEN-LAST:event_RefreshButtonActionPerformed


    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JList<LocalConversationInfo> AvlChtRmList;
    private javax.swing.JButton CancelButton;
    private javax.swing.JButton CreateButton;
    private javax.swing.JButton CreateChatRoomButton;
    private javax.swing.JButton JoinButton;
    private javax.swing.JButton LogoutButton;
    private javax.swing.JTextField NChtRmNmField;
    private javax.swing.JButton QuitButton;
    private javax.swing.JButton RefreshButton;
    private javax.swing.JLabel TimeLabel;
    private javax.swing.JLabel Username;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JPanel jPanel2;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JTabbedPane jTabbedPane1;
    // End of variables declaration//GEN-END:variables
}
