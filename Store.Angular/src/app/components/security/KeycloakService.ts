﻿import { Injectable } from '@angular/core';
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
      realm: 'cocomestores',
      clientId: 'store-client',
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
    if (username === 'storemanager') {
      this.apiUri = 'https://localhost:7001/api';
    } else if (username === 'storemanager2') {
      this.apiUri = 'https://localhost:7002/api';
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
