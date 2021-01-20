package com.pantrytrackers.walmartapi;

import com.microsoft.azure.functions.ExecutionContext;
import com.microsoft.azure.functions.HttpMethod;
import com.microsoft.azure.functions.HttpRequestMessage;
import com.microsoft.azure.functions.HttpResponseMessage;
import com.microsoft.azure.functions.HttpStatus;
import com.microsoft.azure.functions.annotation.AuthorizationLevel;
import com.microsoft.azure.functions.annotation.FunctionName;
import com.microsoft.azure.functions.annotation.HttpTrigger;

import org.json.simple.parser.ParseException;

import java.util.Optional;

/**
 * Azure Functions with HTTP Trigger.
 */
public class Function {
	/**
     * This function listens at endpoint "/api/HttpExample". Two ways to invoke it using "curl" command in bash:
     * 1. curl -d "HTTP Body" {your host}/api/HttpExample
     * 2. curl "{your host}/api/HttpExample?name=HTTP%20Query"
     */
    @FunctionName("HttpExample")
    public HttpResponseMessage run(
            @HttpTrigger(
                name = "req",
                methods = {HttpMethod.POST},
                authLevel = AuthorizationLevel.ANONYMOUS)
                HttpRequestMessage<Optional<String>> request,
            final ExecutionContext context) {
        context.getLogger().info("Java HTTP trigger processed a request.");

        final String bodyObject = request.getBody().orElse("");
        String privateKey = System.getenv("WalmartPrivateKey");
        String consumerId = System.getenv("WalmartConsumerId").trim().toLowerCase();
        Long privateKeyVersion = Long.parseLong(System.getenv("WalmartPrivateKeyVersion"));
        
        try {
            WalmartSignatureRequest requestObj = new WalmartSignatureRequest(bodyObject);

            if(!requestObj.ConsumerID.equals(consumerId)) {
                return request.createResponseBuilder(HttpStatus.UNAUTHORIZED).body("Invalid Consumer ID").build();
            }

            if(!privateKeyVersion.equals(requestObj.PrivateKeyVersion)) {
                return request.createResponseBuilder(HttpStatus.BAD_REQUEST).body("The private specified key version is incorrect or the server's key has been updated recently.").build();
            }

            WalmartSignature output = new SignatureGenerator().getSignature(consumerId, privateKeyVersion, privateKey);
            String bodyValue = output.toJsonString();
            //String bodyValue = output.Signature + "\n" + output.TimeStamp + "\n";
    
            return request.createResponseBuilder(HttpStatus.OK).body(bodyValue).build();
        }
        catch(ParseException ex) {
            return request.createResponseBuilder(HttpStatus.BAD_REQUEST).body("Invalid Request Body -- Must be { \"ConsumerId\":\"String\", \"PrivateKeyVersion\":\"Integer\" }").build();
        }
    }
}
