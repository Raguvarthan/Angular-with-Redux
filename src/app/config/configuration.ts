import * as env from '../../environments/environment';

export const apiUrl = {
    serverUrl: '',
    authServer: '',
    tokenServer: ''
};

export const relatedLogic = {
    productFor: true, 
    productType: true,
    productDesign: false
};

export const tax = {
    estimatedTax: .05
}
// ***dev */
apiUrl.serverUrl = 'http://192.168.0.112:4000/api/';
apiUrl.authServer = 'http://192.168.0.112:4000/api/auth';
apiUrl.tokenServer = 'http://192.168.0.112:4001/api/';

// ***prod */
// if (!env.environment) {
// apiUrl.serverUrl = 'http://192.168.0.112:4000/api/';
// apiUrl.authServer = 'http://192.168.0.112:4000/api/auth';
// apiUrl.tokenServer = 'http://192.168.0.112:4001/api/';
// }

export class Configuration {
    public apiKey: string;
    public username: string;
    public password: string;
    public accessToken: string | (() => string);
}

export const COLLECTION_FORMATS = {
    csv: ',',
    tsv: '   ',
    ssv: ' ',
    pipes: '|'
};
