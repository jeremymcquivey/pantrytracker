package com.pantrytrackers.walmartapi;

import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

public class WalmartSignatureRequest {
    public String ConsumerID;

    public Long PrivateKeyVersion;

    public WalmartSignatureRequest() {
    }

    public WalmartSignatureRequest(String json) throws ParseException {
        try {
            JSONParser parser = new JSONParser();
            JSONObject jsonObj = (JSONObject) parser.parse(json);

            String consumerId = (String)jsonObj.get("ConsumerID");
            ConsumerID = consumerId != null ? consumerId.trim().toLowerCase() : "";
            
            PrivateKeyVersion = (Long)jsonObj.get("PrivateKeyVersion");
        } catch (ParseException e) { throw e; }
    }
}
