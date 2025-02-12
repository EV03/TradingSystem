import { Injectable } from '@angular/core';
import Keycloak from 'keycloak-js';

@Injectable({
  providedIn: 'root',
})
export class KeycloakService {
  private keycloakAuth: Keycloak;
  private apiUri: string = '';

  constructor() {
    this.keycloakAuth = new Keycloak({
      url: 'http://localhost:8080/',
      realm: 'cocomeenterprise',
      clientId: 'enterprise-client',
    });
  }

  init(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.keycloakAuth
        .init({ onLoad: 'login-required' })
        .then(() => {
          if (this.keycloakAuth.authenticated) {
            console.log('User authenticated');
            this.setApiUri();
            resolve();
          } else {
            console.warn('User not authenticated, redirecting to login');
            this.keycloakAuth.login();
          }
        })
        .catch((err) => reject(err));
    });
  }

  private setApiUri(): void {
    const username = this.keycloakAuth.tokenParsed?.['preferred_username'];
    if (username === 'enterprisemanager') {
      this.apiUri = ' https://localhost:7058/api';
    } else if (username === 'enterprisemanager2') {
      this.apiUri = '';
    } else {
      this.apiUri = '';
    }
  }
  getApiUri(): string {
    return this.apiUri;
  }


  getToken(): string | undefined {
    return this.keycloakAuth.token;
  }

  logout(): void {
    this.keycloakAuth.logout({ redirectUri: window.location.origin });
  }
}
