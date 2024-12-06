// Code generated by Microsoft (R) TypeSpec Code Generator.

package petstore.implementation;

import io.clientcore.core.annotation.ServiceInterface;
import io.clientcore.core.http.RestProxy;
import io.clientcore.core.http.annotation.BodyParam;
import io.clientcore.core.http.annotation.HeaderParam;
import io.clientcore.core.http.annotation.HostParam;
import io.clientcore.core.http.annotation.HttpRequestInformation;
import io.clientcore.core.http.annotation.PathParam;
import io.clientcore.core.http.annotation.UnexpectedResponseExceptionDetail;
import io.clientcore.core.http.exception.HttpResponseException;
import io.clientcore.core.http.models.HttpMethod;
import io.clientcore.core.http.models.RequestOptions;
import io.clientcore.core.http.models.Response;
import io.clientcore.core.util.binarydata.BinaryData;
import petstore.Insurance;
import petstore.PetStoreError;

/**
 * An instance of this class provides access to all the operations defined in OwnerInsurances.
 */
public final class OwnerInsurancesImpl {
    /**
     * The proxy service used to perform REST calls.
     */
    private final OwnerInsurancesService service;

    /**
     * The service client containing this operation class.
     */
    private final PetStoreClientImpl client;

    /**
     * Initializes an instance of OwnerInsurancesImpl.
     * 
     * @param client the instance of the service client containing this operation class.
     */
    OwnerInsurancesImpl(PetStoreClientImpl client) {
        this.service = RestProxy.create(OwnerInsurancesService.class, client.getHttpPipeline());
        this.client = client;
    }

    /**
     * The interface defining all the services for PetStoreClientOwnerInsurances to be used by the proxy service to
     * perform REST calls.
     */
    @ServiceInterface(name = "PetStoreClientOwnerI", host = "{endpoint}")
    public interface OwnerInsurancesService {
        @HttpRequestInformation(
            method = HttpMethod.GET,
            path = "/owners/{ownerId}/insurance",
            expectedStatusCodes = { 200 })
        @UnexpectedResponseExceptionDetail(exceptionBodyClass = PetStoreError.class)
        Response<Insurance> getSync(@HostParam("endpoint") String endpoint, @PathParam("ownerId") long ownerId,
            @HeaderParam("Accept") String accept, RequestOptions requestOptions);

        @HttpRequestInformation(
            method = HttpMethod.PATCH,
            path = "/owners/{ownerId}/insurance",
            expectedStatusCodes = { 200 })
        @UnexpectedResponseExceptionDetail(exceptionBodyClass = PetStoreError.class)
        Response<Insurance> updateSync(@HostParam("endpoint") String endpoint, @PathParam("ownerId") long ownerId,
            @HeaderParam("Content-Type") String contentType, @HeaderParam("Accept") String accept,
            @BodyParam("application/json") BinaryData properties, RequestOptions requestOptions);
    }

    /**
     * Gets the singleton resource.
     * 
     * @param ownerId The ownerId parameter.
     * @param requestOptions The options to configure the HTTP request before HTTP client sends it.
     * @throws HttpResponseException thrown if the service returns an error.
     * @return the singleton resource.
     */
    public Response<Insurance> getWithResponse(long ownerId, RequestOptions requestOptions) {
        final String accept = "application/json";
        return service.getSync(this.client.getEndpoint(), ownerId, accept, requestOptions);
    }

    /**
     * Updates the singleton resource.
     * <p><strong>Request Body Schema</strong></p>
     * 
     * <pre>
     * {@code
     * InsuranceUpdate
     * }
     * </pre>
     * 
     * @param ownerId The ownerId parameter.
     * @param properties The properties parameter.
     * @param requestOptions The options to configure the HTTP request before HTTP client sends it.
     * @throws HttpResponseException thrown if the service returns an error.
     * @return the response.
     */
    public Response<Insurance> updateWithResponse(long ownerId, BinaryData properties, RequestOptions requestOptions) {
        final String contentType = "application/json";
        final String accept = "application/json";
        return service.updateSync(this.client.getEndpoint(), ownerId, contentType, accept, properties, requestOptions);
    }
}
