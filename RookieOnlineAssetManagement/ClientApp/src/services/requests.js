import axios from "axios";
import { UrlBackEnd } from "../Constants/oidc-config";

const config = {
    baseURL: UrlBackEnd
}

class RequestService {
    axios;

    constructor() {
        this.axios = axios.create(config);
    }
}

export default new RequestService();
