
import { Validator } from "../utils/validator";
import appConfig from "@/app-config";
import Common from "../utils/common";
import { GetRequest } from "./request";

class WebSocketProxy{
    constructor(url){
        try {
            this.originUrl = url;
            let socketUrl = Common.removeTrailingSlash(appConfig.getSocketUrl());
            let token = localStorage.getItem('access-token');
            this.url = socketUrl + '/' + this.originUrl + "?token=" + token;
            this.socket = new WebSocket(this.url);
            this.handlers = [];
            this.errorers = [];
            this.socket.onmessage = this.resolveOnMessage.bind(this);
            this.socket.onerror = this.resolveOnError.bind(this);
        } catch (e){
            // try to refresh token
            this.retryConnectSocket();
        }
    }


    async retryConnectSocket(){
        try {
            await new GetRequest().checkLogedIn();
            let token = localStorage.getItem('access-token');
            let socketUrl = Common.removeTrailingSlash(appConfig.getSocketUrl());
            this.url = socketUrl + '/' + this.originUrl + "?token=" + token;
            this.socket = new WebSocket(this.url);
            this.socket.onmessage = this.resolveOnMessage.bind(this);
            this.socket.onerror = this.resolveOnError.bind(this);
        } catch (e){
            console.error(e);
        }
    }


    /**
     * This method is used to send a message to the server
     * @param {object} message - message to be sent 
     * @returns nothing
     * @Created PhucTV 1/6/24
     * @ModifiedBy None
     * @example new WebSocketProxy('ws://localhost:8080').send({name: 'PhucTV', age: 24});
     */
    send(message){
        try {
            if (Validator.isEmptyOrSpace(message)) return;
            if (this.socket == null) return;
            
            let value = JSON.stringify(message);
            this.socket.send(value);
        } catch (error){
            console.error(error);
        }
    }


    /**
     * This method is used to register a callback function to handle incoming messages
     * @param {function} callback - callback function to handle incoming messages
     * @returns nothing
     * @Created PhucTV 1/6/24
     * @ModifiedBy None
     * @example new WebSocketProxy('ws://localhost:8080').registerOnMessage((message) => console.log(message));
     */
    registerOnMessage(callback){
        try {
            if (Validator.isEmptyOrSpace(callback)) return;
            this.handlers.push(callback);
        } catch (error){
            console.error(error);
        }
    }


    /**
     * This method is used to register a callback function to handle incoming errors
     * @param {function} callback - callback function to handle incoming errors
     * @returns nothing
     * @Created PhucTV 1/6/24
     * @ModifiedBy None
     * @example new WebSocketProxy('ws://localhost:8080').registerOnError((error) => console.error(error));
     */
    registerOnError(callback){
        try {
            if (Validator.isEmptyOrSpace(callback)) return;
            this.errorers.push(callback);
        } catch (error){
            console.error(error);
        }
    }


    /**
     * This method is used to resolve incoming messages
     * @param {object} event - incoming message
     * @returns nothing
     * @Created PhucTV 1/6/24
     * @ModifiedBy None
     */ 
    async resolveOnMessage(event){
        try {
            if (Validator.isEmptyOrSpace(event)) return;
            let message = JSON.parse(event.data);
            await Promise.all(this.handlers.map(handler => handler(message)));
        } catch (error){
            console.error(error);
        }
    }


    /**
     * This method is used to resolve incoming errors
     * @param {object} event - incoming error
     * @returns nothing
     * @Created PhucTV 1/6/24
     * @ModifiedBy None
     */
    resolveOnError(event){
        try {
            if (Validator.isEmptyOrSpace(event)) return;
            this.errorers.map(errorer => errorer(event));
        } catch (error){
            console.error(error);
        }
    }



    /**
     * This method is used to close the connection
     * @returns nothing
     * @Created PhucTV 1/6/24
     * @ModifiedBy None
     */
    close(){
        try {
            this.socket.close();
        } catch (error){
            console.error(error);
        }
    }
}


export default WebSocketProxy;

