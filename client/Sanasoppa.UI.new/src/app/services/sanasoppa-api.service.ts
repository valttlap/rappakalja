//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class Client {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ?? "";
    }

    /**
     * Returns all game sessions
     * @return Returns all game sessions
     */
    getGameSessions(): Observable<GameSessionDto[]> {
        let url_ = this.baseUrl + "/api/Game";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetGameSessions(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetGameSessions(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<GameSessionDto[]>;
                }
            } else
                return _observableThrow(response_) as any as Observable<GameSessionDto[]>;
        }));
    }

    protected processGetGameSessions(response: HttpResponseBase): Observable<GameSessionDto[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(GameSessionDto.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return _observableOf(result200);
            }));
        } else if (status === 500) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("Internal server error", status, _responseText, _headers);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    /**
     * Creates a new game session
     * @return Creates a new game session
     */
    createGameSession(): Observable<GameSessionDto> {
        let url_ = this.baseUrl + "/api/Game";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCreateGameSession(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateGameSession(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<GameSessionDto>;
                }
            } else
                return _observableThrow(response_) as any as Observable<GameSessionDto>;
        }));
    }

    protected processCreateGameSession(response: HttpResponseBase): Observable<GameSessionDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 201) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result201: any = null;
            let resultData201 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result201 = GameSessionDto.fromJS(resultData201);
            return _observableOf(result201);
            }));
        } else if (status === 500) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("Internal server error", status, _responseText, _headers);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    /**
     * Returns a game session by id
     * @return Returns a game session by id
     */
    getGameSessionById(id: string): Observable<GameSessionDto> {
        let url_ = this.baseUrl + "/api/Game/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetGameSessionById(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetGameSessionById(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<GameSessionDto>;
                }
            } else
                return _observableThrow(response_) as any as Observable<GameSessionDto>;
        }));
    }

    protected processGetGameSessionById(response: HttpResponseBase): Observable<GameSessionDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = GameSessionDto.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status === 404) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("Game session not found", status, _responseText, _headers);
            }));
        } else if (status === 500) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("Internal server error", status, _responseText, _headers);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    /**
     * Returns all players in a game session
     * @return Returns all players in a game session
     */
    getPlayersByGameSessionId(id: string): Observable<PlayerDto[]> {
        let url_ = this.baseUrl + "/api/Game/{id}/players";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetPlayersByGameSessionId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetPlayersByGameSessionId(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<PlayerDto[]>;
                }
            } else
                return _observableThrow(response_) as any as Observable<PlayerDto[]>;
        }));
    }

    protected processGetPlayersByGameSessionId(response: HttpResponseBase): Observable<PlayerDto[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(PlayerDto.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return _observableOf(result200);
            }));
        } else if (status === 404) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("Game session not found", status, _responseText, _headers);
            }));
        } else if (status === 500) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("Internal server error", status, _responseText, _headers);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    /**
     * Returns status of the game
     * @return Returns game status
     */
    getStatusByGameSessionId(id: string): Observable<void> {
        let url_ = this.baseUrl + "/api/Game/{id}/status";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetStatusByGameSessionId(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetStatusByGameSessionId(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processGetStatusByGameSessionId(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return _observableOf(null as any);
            }));
        } else if (status === 404) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("Game session not found", status, _responseText, _headers);
            }));
        } else if (status === 500) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("Internal server error", status, _responseText, _headers);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    /**
     * Returns Auth0 Settings
     * @return Returns Auth0 Settings
     */
    getAuth0Settings(): Observable<SettingsDto> {
        let url_ = this.baseUrl + "/api/Settings";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetAuth0Settings(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAuth0Settings(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<SettingsDto>;
                }
            } else
                return _observableThrow(response_) as any as Observable<SettingsDto>;
        }));
    }

    protected processGetAuth0Settings(response: HttpResponseBase): Observable<SettingsDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = SettingsDto.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status === 500) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("Internal server error", status, _responseText, _headers);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }
}

export class GameSessionDto implements IGameSessionDto {
    id!: string;
    joinCode?: string | undefined;
    startTime?: Date | undefined;
    endTime?: Date | undefined;
    ownerId?: string | undefined;
    players?: PlayerDto[];
    rounds?: RoundDto[];

    constructor(data?: IGameSessionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.joinCode = _data["joinCode"];
            this.startTime = _data["startTime"] ? new Date(_data["startTime"].toString()) : <any>undefined;
            this.endTime = _data["endTime"] ? new Date(_data["endTime"].toString()) : <any>undefined;
            this.ownerId = _data["ownerId"];
            if (Array.isArray(_data["players"])) {
                this.players = [] as any;
                for (let item of _data["players"])
                    this.players!.push(PlayerDto.fromJS(item));
            }
            if (Array.isArray(_data["rounds"])) {
                this.rounds = [] as any;
                for (let item of _data["rounds"])
                    this.rounds!.push(RoundDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): GameSessionDto {
        data = typeof data === 'object' ? data : {};
        let result = new GameSessionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["joinCode"] = this.joinCode;
        data["startTime"] = this.startTime ? this.startTime.toISOString() : <any>undefined;
        data["endTime"] = this.endTime ? this.endTime.toISOString() : <any>undefined;
        data["ownerId"] = this.ownerId;
        if (Array.isArray(this.players)) {
            data["players"] = [];
            for (let item of this.players)
                data["players"].push(item.toJSON());
        }
        if (Array.isArray(this.rounds)) {
            data["rounds"] = [];
            for (let item of this.rounds)
                data["rounds"].push(item.toJSON());
        }
        return data;
    }
}

export interface IGameSessionDto {
    id: string;
    joinCode?: string | undefined;
    startTime?: Date | undefined;
    endTime?: Date | undefined;
    ownerId?: string | undefined;
    players?: PlayerDto[];
    rounds?: RoundDto[];
}

export class PlayerDto implements IPlayerDto {
    id!: string;
    name!: string;
    gameSessionId!: string;
    connectionId!: string;
    playerId!: string;

    constructor(data?: IPlayerDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.name = _data["name"];
            this.gameSessionId = _data["gameSessionId"];
            this.connectionId = _data["connectionId"];
            this.playerId = _data["playerId"];
        }
    }

    static fromJS(data: any): PlayerDto {
        data = typeof data === 'object' ? data : {};
        let result = new PlayerDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["gameSessionId"] = this.gameSessionId;
        data["connectionId"] = this.connectionId;
        data["playerId"] = this.playerId;
        return data;
    }
}

export interface IPlayerDto {
    id: string;
    name: string;
    gameSessionId: string;
    connectionId: string;
    playerId: string;
}

export class RoundDto implements IRoundDto {
    id?: string;
    gameSessionId?: string;
    roundNumber?: number;
    leaderId?: string;
    word?: string | undefined;

    constructor(data?: IRoundDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.gameSessionId = _data["gameSessionId"];
            this.roundNumber = _data["roundNumber"];
            this.leaderId = _data["leaderId"];
            this.word = _data["word"];
        }
    }

    static fromJS(data: any): RoundDto {
        data = typeof data === 'object' ? data : {};
        let result = new RoundDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["gameSessionId"] = this.gameSessionId;
        data["roundNumber"] = this.roundNumber;
        data["leaderId"] = this.leaderId;
        data["word"] = this.word;
        return data;
    }
}

export interface IRoundDto {
    id?: string;
    gameSessionId?: string;
    roundNumber?: number;
    leaderId?: string;
    word?: string | undefined;
}

export class SettingsDto implements ISettingsDto {
    domain!: string;
    clientId!: string;
    audience!: string;

    constructor(data?: ISettingsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.domain = _data["domain"];
            this.clientId = _data["clientId"];
            this.audience = _data["audience"];
        }
    }

    static fromJS(data: any): SettingsDto {
        data = typeof data === 'object' ? data : {};
        let result = new SettingsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["domain"] = this.domain;
        data["clientId"] = this.clientId;
        data["audience"] = this.audience;
        return data;
    }
}

export interface ISettingsDto {
    domain: string;
    clientId: string;
    audience: string;
}

export class ApiException extends Error {
    override message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new ApiException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((event.target as any).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}
