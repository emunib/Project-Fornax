/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package client;

/**
 *
 * @author nathanhupka
 */
public class Generic {
    static final String[] Int2MonthTable = {
        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    };
    static String Int2Month(int value){
        return Int2MonthTable[value - 1];
    }
}
