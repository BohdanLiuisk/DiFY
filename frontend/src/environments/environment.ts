export const environment = {
  production: false,
  coreUrl: 'http://localhost:5065',
  authConfig: {
    client_id: 'ro.client',
    client_secret: 'secret'
  },
  peerOptions: {
    key: 'peerjs',
    host: 'localhost',
    port: 443,
    secure: false,
    path: '/',
    debug: 1,
    config: {
      iceServers: [
        { urls: 'stun:stun1.l.google.com:19302' },
        { urls: 'stun:stun2.l.google.com:19302' }
      ],
    },
  },
  hubs: {
    callHub: {
      hubName: "call",
      hubUrl: "http://localhost:5065/hubs/call"
    },
    difyHub: {
      hubName: "dify",
      hubUrl: "http://localhost:5065/hubs/dify"
    }
  }
};
