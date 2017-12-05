/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package client;

import Common.Message;

/**
 *
 * @author nathanhupka
 */
public class MessageHolder {
    public final String sender;
    public final String message;
    public final String time;
    
    public MessageHolder(Message nmessage){
        sender = nmessage.getUser();
        message = nmessage.getContent();
        time = nmessage.getTime();
    }
    
    @Override
    public String toString(){
        String[] timeArray = time.split(":");
        StringBuilder stime = new StringBuilder();
        stime.append(Generic.Int2Month(Integer.parseInt(timeArray[1]))).append(' ').append(timeArray[2]);
        stime.append("/").append(timeArray[0].substring(timeArray[0].length() - 2));
        for(int i = 3; i < timeArray.length; i++){
            stime.append(":").append(timeArray[i]);
        }
        return  stime.toString() + " : " + sender + " : " + message;
    }
}
