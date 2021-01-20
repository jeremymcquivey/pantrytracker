package com.pantrytrackers.walmartapi;

import java.io.ObjectStreamException;
import java.security.KeyRep;
import java.security.PrivateKey;
import java.security.Signature;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;
import java.util.SortedSet;
import java.util.TreeSet;
import java.util.Base64;

public class SignatureGenerator {
    private static int SignatureTimeToLive = 1000 * 180; 
    private static String ConsumerIdHeader = "WM_CONSUMER.ID";
    private static String ConsumerTimestampHeader = "WM_CONSUMER.INTIMESTAMP";
    private static String ConsumerKeyVersion = "WM_SEC.KEY_VERSION";
    private static String EncryptionMethodName = "SHA256WithRSA";

    public WalmartSignature getSignature(String consumerId, Long privateKeyVersion, String privateKey) {
        WalmartSignature returnData = new WalmartSignature();
        returnData.TimeStamp = System.currentTimeMillis();
        returnData.ValidUntil = returnData.TimeStamp + SignatureTimeToLive;
        returnData.ConsumerId = consumerId;

        Map<String, String> map = new HashMap<>();
        map.put(ConsumerIdHeader, consumerId);
        map.put(ConsumerTimestampHeader, Long.toString(returnData.TimeStamp));
        map.put(ConsumerKeyVersion, Long.toString(privateKeyVersion));

        try {
            String[] array = canonicalize(map);
            returnData.Signature = generateSignature(privateKey, array[1]);
        } catch(Exception e) { }

        return returnData;
    }

    public String generateSignature(String key, String stringToSign) throws Exception {
        ServiceKeyRep keyRep = new ServiceKeyRep(KeyRep.Type.PRIVATE, "RSA", "PKCS#8", Base64.getDecoder().decode(key));
        Signature signatureInstance = Signature.getInstance(EncryptionMethodName);
        byte[] bytesToSign = stringToSign.getBytes("UTF-8");

        signatureInstance.initSign((PrivateKey) keyRep.readResolve());
        signatureInstance.update(bytesToSign);

        return Base64.getEncoder().encodeToString(signatureInstance.sign());
    }
    
    protected String[] canonicalize(Map<String, String> headersToSign) {
        StringBuffer canonicalizedStrBuffer = new StringBuffer();
        StringBuffer parameterNamesBuffer = new StringBuffer();
        Set<String> keySet = headersToSign.keySet();

        // Create sorted key set to enforce order on the key names
        SortedSet<String> sortedKeySet=new TreeSet<String>(keySet);
        for (String key :sortedKeySet) {
            Object val=headersToSign.get(key);
            parameterNamesBuffer.append(key.trim()).append(";");
            canonicalizedStrBuffer.append(val.toString().trim()).append("\n");
        }
        return new String[] { parameterNamesBuffer.toString(), canonicalizedStrBuffer.toString() };
    }

    public class ServiceKeyRep extends KeyRep  {
        private static final long serialVersionUID = -7213340660431987616L;
        public ServiceKeyRep(Type type, String algorithm, String format, byte[] encoded) {
            super(type, algorithm, format, encoded);
        }
        protected Object readResolve() throws ObjectStreamException {
            return super.readResolve();
        }
    }
}
