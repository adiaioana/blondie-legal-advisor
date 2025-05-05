
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "route": "/"
  },
  {
    "renderMode": 2,
    "route": "/analyzer"
  },
  {
    "renderMode": 2,
    "route": "/register"
  },
  {
    "renderMode": 2,
    "route": "/login"
  },
  {
    "renderMode": 2,
    "route": "/my-account"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 499, hash: '74c80f72b936fbc723159e10a7db063af47cd77e3c48207afef5660ba558ee79', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1012, hash: 'af5a861a2ae14f7a0a4635f339ea433e13f8a02c2733b4223a9ffdfd91136a8b', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'analyzer/index.html': {size: 10660, hash: '687678bd126c514f0d1dd84cadc990ee8c0f9f8ac96e8bf05b24f6d081eb0406', text: () => import('./assets-chunks/analyzer_index_html.mjs').then(m => m.default)},
    'index.html': {size: 11050, hash: '1f60abb33711f38c55074fbe83200a8de384067eb2245c269b439e3e38e81665', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'my-account/index.html': {size: 10660, hash: '687678bd126c514f0d1dd84cadc990ee8c0f9f8ac96e8bf05b24f6d081eb0406', text: () => import('./assets-chunks/my-account_index_html.mjs').then(m => m.default)},
    'register/index.html': {size: 11247, hash: 'c8b8eebc7c79e35728403a39a89aa6d17b5bf4c6412e2c48d0dd1d83cc94c1c2', text: () => import('./assets-chunks/register_index_html.mjs').then(m => m.default)},
    'login/index.html': {size: 10660, hash: '687678bd126c514f0d1dd84cadc990ee8c0f9f8ac96e8bf05b24f6d081eb0406', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'styles-5INURTSO.css': {size: 0, hash: 'menYUTfbRu8', text: () => import('./assets-chunks/styles-5INURTSO_css.mjs').then(m => m.default)}
  },
};
