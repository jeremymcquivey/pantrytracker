package com.pantrytrackers.walmartapi;

import java.util.LinkedHashMap;
import java.util.Map;

import org.json.simple.JSONValue;

public class WalmartSignature {
    public String Signature;
    public String ConsumerId;
    public long TimeStamp;
    public long ValidUntil;

    public String toJsonString() {
        Map<String,Object> obj1 = new LinkedHashMap<String,Object>();
        obj1.put("Signature", Signature);
        obj1.put("TimeStamp", TimeStamp);
        obj1.put("ValidUntil", ValidUntil);
        obj1.put("ConsumerId", ConsumerId);

        return JSONValue.toJSONString(obj1);
    }
}
